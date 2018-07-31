function PostAjax(datasource, iddatatarget, urltarget) {
    $.ajax({
        type: 'POST',
        url: urltarget,

        dataType: 'json',

        data: { param: datasource },


        success: function (elements) {


            $.each(elements, function (i, element) {
                //console.log("element V: " + element.Value);
                //console.log("element T: " + element.Text);
                iddatatarget.append('<option value="' + element.Value + '">' +
                    element.Text + '</option>');

            });
        },
        error: function (ex) {
            alert('Failed to retrieve elements.' + ex);
        }
    });
    return completed = 1;
};

$(document).ready(function () {
    var $loading = $('.loader').hide();
    $(document)
        .ajaxStart(function () {
            $loading.show();
        })
        .ajaxStop(function () {
            $loading.hide();
        });
    //jQuery.validator.methods.date = function () { return Globalize.parseDate(value); };
    $('input[type = datetime]').datepicker({
        dateFormat: "dd/mm/yy",
        buttonImage: "/Content/images/calendar.png",
        showOn: "button",
        //buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-5:+3"
    }).val();
    $('input[type = datetime]').on('click', function () {
        $(this).datepicker('show');
    });
    $('.ui-datepicker-trigger[type = button]').css({
        'position': 'absolute',        
        'margin-left': $('input[type = datetime]').innerWidth() - 40
    });
    $('.form-group .ui-datepicker-trigger[type = button]').css('top', 4);
    if ($(window).width() < 768) {
        $('dd .ui-datepicker-trigger[type = button]').css('top', 40);
    } else {
        $('dd .ui-datepicker-trigger[type = button]').css('top', 14);
    }

    $(window).on('resize', function () {
        $('.ui-datepicker-trigger[type = button]').css({
            'position': 'absolute',
            'margin-left': $('input[type = datetime]').innerWidth() - 40
        });
        $('.form-group .ui-datepicker-trigger[type = button]').css('top', 4);
        
        if ($(window).width() < 768) {
            $('dd .ui-datepicker-trigger[type = button]').css('top', 40);
        } else {
            $('dd .ui-datepicker-trigger[type = button]').css('top', 14);
        }
    })

    $('body').on('click', function (event) {
        if ($("#LstNavJobs option").length == 0) {
            //console.log('Cero');
            $("#BtnAddJob").addClass('disabled');
        } else {
            //console.log('No Cero');
            $("#BtnAddJob").removeClass('disabled')
        }
        if ($("#LstCawJobs option").length == 0) {
            //console.log('Cero');
            $("#BtnRemoveJob").addClass('disabled');
        } else {
            //console.log('No Cero');
            $("#BtnRemoveJob").removeClass('disabled')
        }
        //console.log(event.target.id);
        //console.log(this);
        //debugger;
        if (event.target.id == "caw_notes") {
            $('#caw_notes').css({
                "max-width": 500,
                "height": 300,
                "transition": "all ease-in 0.4s"
            });
            //$('#caw_notes').focus(function () { $('#caw_notes').css("max-width", 500) });
            //console.log('Note');
        } else {
            $('#caw_notes').css({
                "max-width": 280,
                "height": "auto",
                "transition": "all ease-out 0.4s"
            });
            //$('#caw_notes').removeAttr('style');
            //debugger
            //console.log(event.target);
        }
        
        
    });

    //$('input[name=caw_archplan]').on('change', function () {
    //    //console.log($(this).val());
    //    $('.container.file-uploader').toggleClass('anime-hide', 500);
    //})
    //$(document).on('click', '#pdigital, #pphysical', function () {
    //    if ($('.container.file-uploader').hasClass('anime-hide')) {
    //        $('.container.file-uploader').removeClass('anime-hide');
    //    }
    //});
    $('#radio-arch-modes-container').on('change', function (event) {
        console.log(event.target.id);
        if (event.target.id == 'pphysical') {
            $('.container.file-uploader').addClass('anime-hide');
            $('#btnArchive').removeAttr('disabled');
        } else {
            $('.container.file-uploader').removeClass('anime-hide');
        }
    })

    $(function () {

        $("#BtnAddJob,#BtnRemoveJob").click(function (event) {

            console.log('Click');
            var ID = $(event.target).attr("ID");
            var ChooseFrom = ID == "BtnAddJob" ? "#LstNavJobs" : "#LstCawJobs";
            var moveTo = ID == "BtnAddJob" ? "#LstCawJobs" : "#LstNavJobs";
            var SelectData = $(ChooseFrom + " :selected").toArray();
            //console.log('ID:' + ID);
            //console.log(ChooseFrom);
            //console.log(moveTo);
            //console.log(SelectData);
            $(moveTo).append(SelectData);
            SelectData.remove;            
        });
    });
       
    

});
//Function to select all Listbox items
function selectDeselect(listid, status) {
    var listb = document.getElementById(listid);
    var len = listb.options.length;
    for (var i = 0; i < len; i++) {
        listb.options[i].selected = status;
    }
}