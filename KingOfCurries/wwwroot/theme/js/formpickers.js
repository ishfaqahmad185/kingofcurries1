(function($) {
  'use strict';
  if ($("#timepicker-example").length) {
      $('#timepicker-example').datetimepicker({
          format: 'LT'
     //   format: 'HH:mm',
        
    });
  }
    if ($("#timepicker-example2").length) {
        $('#timepicker-example2').datetimepicker({
            format: 'LT'
           //  format: 'HH:mm',
           
           
        });
    }

  if ($(".color-picker").length) {
    $('.color-picker').asColorPicker();
  }
  if ($("#datepicker-popup").length) {
    $('#datepicker-popup').datepicker({
      enableOnReadonly: true,
      todayHighlight: true,
    });
  }
    if ($(".datepicker-popup").length) {
        $('.datepicker-popup').datepicker({
            enableOnReadonly: true,
            todayHighlight: true,
        });
    }

    if ($("#datepicker-popup2").length) {
        $('#datepicker-popup2').datepicker({
            enableOnReadonly: true,
            todayHighlight: true,
        });
    }
    if ($("#datepicker-popup3").length) {
        $('#datepicker-popup3').datepicker({
            enableOnReadonly: true,
            todayHighlight: true,
        });
    }
  if ($("#inline-datepicker").length) {
    $('#inline-datepicker').datepicker({
      enableOnReadonly: true,
      todayHighlight: true,
    });
  }
  if ($(".datepicker-autoclose").length) {
    $('.datepicker-autoclose').datepicker({
      autoclose: true
    });
  }
  if ($('input[name="date-range"]').length) {
    $('input[name="date-range"]').daterangepicker();
  }
  if($('.input-daterange').length) {
    $('.input-daterange input').each(function() {
      $(this).datepicker('clearDates');
    });
    $('.input-daterange').datepicker({});
  }
})(jQuery);