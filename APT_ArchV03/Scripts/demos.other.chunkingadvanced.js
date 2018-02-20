
/* global $, window */

$(function () {
    'use strict';

    var msg_resume = ' bytes already uploaded. To resume upload click start';
    var msg_override = 'File already uploaded. Overwrite anyway?';
    var msg_leave = 'You are currently uploading files. If you leave this page, your upload may not complete successfully.';

    var web_app = '/';                                                   // Name of a web application (usually in full IIS mode). Can be found in Properties/Web/Server/Project-Url. Example: http://localhost/Demo (Name of web application is "Demo")

    // We use the upload handler integrated into Backload:
    // In this example we set an objectContect (id) in the URL query (or as form parameter). The value of the opjectContext parameter   
    // will be added to the path as a sub-folder of the storage root folder (storageRoot\objectContext\file). You can assign a user id
    // to objectContext as user directory. Instead of setting the objectContext client side you can also be set server side, 
    // e.g. server side events (see also Custom Data Provider Demo, 2.2+).
    var url = web_app + 'Backload/FileHandler?objectContext=C5F260DD3787&uploadContext=19';


    // Initialize the jQuery File Upload widget:
    $('#fileupload').fileupload({
        url: url,
        maxChunkSize: 5000000,                                           // Size of file chunks (data packets): 5MB. Note: In a real world scenario this depends on target infrastructure or use cases.
        acceptFileTypes: /(jpg)|(jpeg)|(gsa)|(mp4)|(png)|(gif)|(pdf)$/i,             // Allowed file types

        // the add function is called immediately after a file was added to the client UI list, but not uploaded so far.
        add: function (e, data) {
            var that = this;

            // "getFileInfo" requests meta data for the file just added to the client UI list from the server.
            //   execute: getFileInfo({filename}) (get file or chunk meta data); 
            //   ts: time-stamp (prevents caching)
            $.getJSON(url, { execute: "getFileInfo(" + data.files[0].name + ")", ts: e.timeStamp },
                // Server response
                function (result) {
                    var uuid = data.files[0].size.toString(16);                                             // Create new UUID based on the file size or any other value like user id, etc
                    
                    if (result.files.length > 0) {                                                          // If result.files.length is 0 no previous partial uploads
                        for (var i = 0; i < result.files.length; i++) {                                     // Iterate over the list of partial uploads for this file name, usually only one exist
                            var file = result.files[i];
                            if ((file.extra.chunkInfo != null) && (file.extra.chunkInfo.uuid == uuid)) {    // if chunkinfo is not null and has the same UUID we can resume the upload
                                data.uploadedBytes = file.extra.chunkInfo.uploadedSize;                     // Set number of bytes already uploaded
                                alert(file.extra.chunkInfo.uploadedSize + msg_resume);
                            } else if (data.files[0].size == file.size) {                                   // if true, file already uploaded
                                if (!confirm(msg_override)) {
                                    return;                                                                 // Do not add file to list, because file is already fully uploaded
                                }
                            }
                        }
                    }

                    data.formData = { 'uuid': uuid };                                                       // When uploading the file, the UUID prevents the file to be overwritten by a file with different size
                    $.blueimp.fileupload.prototype.options.add.call(that, e, data);                         // Add file with the UUID as form parameter to client UI list 
                }) 
        },
        send: function (e, data) {
            uploadingFiles += 1;
        },
        always: function (e, data) {
            uploadingFiles -= 1;
        }
    })


    // Warn user if file is currently uploaded. 
    var uploadingFiles = 0;
    $(window).bind('beforeunload', function () {
        if (uploadingFiles > 0) {
            return msg_leave;
        }
    });



    // Optional: Load existing files:
    $('#fileupload').addClass('fileupload-processing');
    $.ajax({
        // Uncomment the following to send cross-domain cookies:
        // xhrFields: {withCredentials: true},
        url: url,
        dataType: 'json',
        context: $('#fileupload')[0]
    }).always(function () {
        $(this).removeClass('fileupload-processing');
    }).done(function (result) {
        $(this).fileupload('option', 'done')
            .call(this, $.Event('done'), { result: result });
    });
});
