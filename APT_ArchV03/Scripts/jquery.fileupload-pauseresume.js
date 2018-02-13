/*
 * jQuery File Upload User Interface Plugin
 * https://github.com/blueimp/jQuery-File-Upload
 *
 * Copyright 2010, Sebastian Tschan
 * https://blueimp.net 
 *
 * Copyright 2016, Steffen Habermehl (added buttons)
 * http://www.backload.org
 *
 * Licensed under the MIT license:
 * http://www.opensource.org/licenses/MIT
 */

/* jshint nomen:false */
/* global define, require, window */

(function (factory) {
    'use strict';
    if (typeof define === 'function' && define.amd) {
        // Register as an anonymous AMD module:
        define([
            'jquery',
            'tmpl',
            './jquery.fileupload-image',
            './jquery.fileupload-audio',
            './jquery.fileupload-video',
            './jquery.fileupload-validate'
        ], factory);
    } else if (typeof exports === 'object') {
        // Node/CommonJS:
        factory(
            require('jquery'),
            require('tmpl')
        );
    } else {
        // Browser globals:
        factory(
            window.jQuery,
            window.tmpl
        );
    }
}(function ($, tmpl) {
    'use strict';

    $.blueimp.fileupload.prototype._specialOptions.push(
        'filesContainer',
        'uploadTemplateId',
        'downloadTemplateId'
    );

    // The UI version extends the file upload widget and adds 
    // pause/resume functionality for chunk uploads to the user interface:

    // Reference to the widget
    // var $baseWidget = $.blueimp.fileupload;
    // var $thisWidget;
    $.widget('blueimp.fileupload', $.blueimp.fileupload, {

        options: {
            // By default, a pause button is displayed only on chunked file uploads.
            // If this option is set to true the pause button is displayed on all uploads. 
            // Default: false
            pauseAll: false
        },


        // Start/Resume button clicked
        _startHandler: function (e) {
            e.preventDefault();

            var $startbutton = $(e.currentTarget),
                $template = $startbutton.closest('.template-upload'),
                data = $template.data('data'),
                $form = (data) ? data.form : $template.closest('form.fileupload'),
                widget = $form.data('blueimp-fileupload') || $form.data('fileupload'),
                pauseAll = widget.options.pauseAll;

            if (data) {
                if (data.chunked || pauseAll) {
                    if (!data.action) data.action = "send";

                    if (data.action == "pause") {
                        data.action = "resume";
                    }
                }

                // Trigger upload
                if (data.submit) {
                    data.submit();
                }
                // this._super(e);

                // Set pause/resume buttons and state
                if (data.chunked || pauseAll) {

                    $startbutton.prop('disabled', false).addClass('hidden');
                    var $startsymbol = $startbutton.children('i');
                    if ($startsymbol.length) $startsymbol.removeClass('glyphicon-upload').addClass('glyphicon-play');
                    var $starttext = $startbutton.children('span');
                    if ($starttext.length) $starttext.text('Resume');

                    var $pausebutton = $template.find('button.pause');
                    if ($pausebutton.length) $pausebutton.removeClass('hidden');
                }
            }
        },


        // Pause button clicked
        _pauseHandler: function (e) {
            e.preventDefault();

            var $pausebutton = $(e.currentTarget),
                $template = $pausebutton.closest('.template-upload'),
                data = $template.data('data'),
                $form = (data) ? data.form : $template.closest('form.fileupload'),
                widget = $form.data('blueimp-fileupload') || $form.data('fileupload'),
                pauseAll = widget.options.pauseAll;

            if (data) {
                // Set pause/resume buttons and state
                if (data.chunked || pauseAll) {
                    $pausebutton.addClass('hidden');
                    var $resumebutton = $template.find('button.start');
                    if ($resumebutton.length) $resumebutton.removeClass('hidden');

                    data.action = "pause";
                }
                // Abort request
                data.abort();
                delete data.jqXHR;
                //data.jqXHR = undefined;
            } 
        },


        // When an upload is paused, jqXHR fires en error. This must be handled
        _onFail: function (jqXHR, textStatus, errorThrown, options) {
            var data = options.context.data('data');
            if (data) {
                if (!data.action) {
                    data.action = "send";
                } else if (data.action == "pause") {
                    //options.action = "pause";
                    //options.errorThrown = data.errorThrown = "abort";
                    //this._trigger('abort', null, data);
                    //this._getXHRPromise(false);
                }

                // Default error
                if (data.action != "pause") {
                    this._super(jqXHR, textStatus, errorThrown, options);
                }
            }
        },


        // Uploads a file in multiple, sequential requests
        // by splitting the file up in multiple blob chunks.
        // If the second parameter is true, only tests if the file
        // should be uploaded in chunks, but does not invoke any
        // upload requests:
        _chunkedUpload: function (options, testOnly) {
            options.uploadedBytes = options.uploadedBytes || 0;

            var that = this,
                file = options.files[0],
                fs = file.size,
                ub = options.uploadedBytes,
                mcs = options.maxChunkSize || fs,
                slice = this._blobSlice,
                dfd = $.Deferred(),
                promise = dfd.promise(),
                jqXHR,
                upload

            // Sets uploaded bytes on chunk resume
            if (!testOnly) {
                var data = options.context.data('data');
                if (data) {
                    data.chunked = (mcs < fs);
                    if (!data.action) data.action = "send";
                    if ((!options.uploadedBytes) || (options.uploadedBytes == 0)) {
                        options.uploadedBytes = (data.uploadedBytes ? data.uploadedBytes : 0);
                        ub = options.uploadedBytes;
                    }
                }
            }

            if (!(this._isXHRUpload(options) && slice && (ub || mcs < fs)) ||
                    options.data) {
                return false;
            }
            if (testOnly) {
                return true;
            }
            if (ub >= fs) {
                file.error = options.i18n('uploadedBytes');
                return this._getXHRPromise(
                    false,
                    options.context,
                    [null, 'error', file.error]
                );
            }
            // The chunk upload method:
            upload = function () {
                // Clone the options object for each chunk upload:
                var o = $.extend({}, options),
                    currentLoaded = o._progress.loaded,
                    data = o.context.data('data'),
                    resume = true;

                if (console) console.log("Upload state: " + o._progress.loaded);
                o.blob = slice.call(
                    file,
                    ub,
                    ub + mcs,
                    file.type
                );
                // Store the current chunk size, as the blob itself
                // will be dereferenced after data processing:
                o.chunkSize = o.blob.size;
                // Expose the chunk bytes position range:
                o.contentRange = 'bytes ' + ub + '-' +
                    (ub + o.chunkSize - 1) + '/' + fs;
                // Process the upload data (the blob and potential form data):
                that._initXHRData(o);
                // Add progress listeners for this chunk upload:
                that._initProgressListener(o);
                // resumed chunk upload
                if (data && data.action == "resume") {
                    resume = that._trigger('chunkresume', null, o)
                    data.action = "send";
                    o.action = "send";
                }
                jqXHR = (resume && (that._trigger('chunksend', null, o) !== false && $.ajax(o)) ||
                        that._getXHRPromise(false, o.context))
                    .done(function (result, textStatus, jqXHR) {
                        ub = that._getUploadedBytes(jqXHR) ||
                            (ub + o.chunkSize);
                        // Create a progress event if no final progress event
                        // with loaded equaling total has been triggered
                        // for this chunk:
                        if (currentLoaded + o.chunkSize - o._progress.loaded) {
                            that._onProgress($.Event('progress', {
                                lengthComputable: true,
                                loaded: ub - o.uploadedBytes,
                                total: ub - o.uploadedBytes
                            }), o);
                        }
                        
                        // Save uploaded bytes state for pause/resume
                        var data = o.context.data('data');
                        if (data) data.uploadedBytes = ub;

                        options.uploadedBytes = o.uploadedBytes = ub;
                        o.result = result;
                        o.textStatus = textStatus;
                        o.jqXHR = jqXHR;
                        that._trigger('chunkdone', null, o);
                        that._trigger('chunkalways', null, o);
                        if (data.action && (data.action == "pause")) return;
                        if (ub < fs) {
                            // File upload not yet complete,
                            // continue with the next chunk:
                            upload();
                        } else {
                            dfd.resolveWith(
                                o.context,
                                [result, textStatus, jqXHR]
                            );
                        }
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        o.jqXHR = jqXHR;
                        var data = o.context.data('data');
                        if (!data.action) data.action = "send";
                        if (data.action == "pause") {
                            o.action = "pause";
                            o.textStatus = "pause";
                            o.errorThrown = undefined;
                            that._trigger('chunkpause', null, o);
                        } else {
                            o.textStatus = textStatus;
                            o.errorThrown = errorThrown;
                            that._trigger('chunkfail', null, o);
                        }

                        that._trigger('chunkalways', null, o);
                        dfd.rejectWith(
                            o.context,
                            [jqXHR, textStatus, errorThrown]
                        );
                    });
            };
            this._enhancePromise(promise);
            promise.abort = function () {
                return jqXHR.abort();
            };
            upload();
            return promise;
        },


        // Attach pause event handler
        _initEventHandlers: function () {
            this._super();
            this._on(this.options.filesContainer, {
                'click .pause': this._pauseHandler
            });
        }


        //_addConvenienceMethods: function (e, data) {
        //    this._super(e, data);

        //    var pauseAll = $.blueimp.fileupload.prototype.options.pauseAll;

        //    data.pause = function () {
        //        if (data.chunked || pauseAll) {
        //            if (data.context && data.context.length > 0) {
        //                var $pausebutton = data.context.find('button.pause');
        //                if ($pausebutton.length > 0) $pausebutton.click();
        //            } else {
        //                if (!data.action) data.action = "send";
        //                if (data.chunked) data.action = "pause";

        //                // Trigger abort
        //                data.abort();
        //            }
        //        }
        //    },

        //    data.resume = function () {
        //        var pauseAll = $.blueimp.fileupload.prototype.options.pauseAll;

        //        if (data.chunked || pauseAll) {
        //            if (data.context && data.context.length > 0) {
        //                var $resumebutton = data.context.find('button.start');
        //                if ($resumebutton.length > 0) $resumebutton.click();
        //            } else {

        //                if (!data.action) data.action = "send";
        //                if (data.action == "pause") data.action = "resume";

        //                // Trigger upload
        //                if (data.submit) data.submit();
        //            }
        //        }
        //    }
        //}


        //// Widget creation
        //_create: function () {
        //    this._super();
        //    $thisWidget = this;
        //},

    });
}));
