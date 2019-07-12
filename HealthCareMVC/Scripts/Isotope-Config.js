// init Isotope
var $grid = $('.all-items').isotope({
    itemSelector: '.item',
    layoutMode: 'fitRows',
    getSortData: {
        filename: '.file-name',
        filedate: '[data-ticks]'
    }
});

// bind filter button click
$('.filters-button-group').on('click', 'button', function () {
    var filterValue = $(this).attr('data-filter');
    $grid.isotope({ filter: filterValue });
});

// bind sort button click
$('.sort-button-group').on('click', 'button', function () {

    /* Get the element name to sort */
    var sortValue = $(this).attr('data-sort-value');

    /* Get the sorting direction: asc||desc */
    var direction = $(this).attr('data-sort-direction');

    /* convert it to a boolean */
    var isAscending = (direction == 'asc');
    var newDirection = (isAscending) ? 'desc' : 'asc';

    /* pass it to isotope */
    $grid.isotope({ sortBy: sortValue, sortAscending: isAscending });

    $(this).attr('data-sort-direction', newDirection);

    var span = $(this).find('.glyphicon');
    span.toggleClass('glyphicon-chevron-up glyphicon-chevron-down');

});