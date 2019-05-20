/* Fills dropdown button with selected value */
$('.dropdown-menu').on('click', 'a', function(e) {
    e.preventDefault();
    var text = $(this).html();
    var htmlText = text + ' <span class="caret"></span>';
    $(this).closest('.dropdown').find('.dropdown-toggle').html(htmlText);
    // Set value of button to be value from li
    $(this).closest('.dropdown').find('.dropdown-toggle').val($(this)[0].attributes.value.value);
});