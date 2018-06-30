/* eslint-disable no-param-reassign */

export default (ko, $) => {
    ko.bindingHandlers.detailsClick = {
        // init: (element, valueAccessor, allBindings, viewModel, bindContext) => { },
        // update: (element, valueAccessor, allBindings, viewModel, bindContext) => { }
        init: (element, valueAccessor, allBindings, viewModel, bindContext) => {
            $(element).on('click', () => {
                const detailsElement = $(ko.unwrap(valueAccessor)());
                const selectedRow = $(element).parents('tr:first');
                const setSelectedRow = allBindings.get('selectRow');
                const selectedClass = allBindings.get('selectedClass');
                const rowId = allBindings.get('rowId');

                const jElem = $(element);
                if (jElem.hasClass('active-details')) {
                    detailsElement.hide();
                    $(`.${selectedClass}`).removeClass(selectedClass);
                    jElem.removeClass('active-details');
                }
                else {
                    // clear existing visible selection
                    detailsElement.hide();
                    $(`.${selectedClass}`).removeClass(selectedClass);
                    $('.active-details').removeClass('active-details');

                    // find the row and bind it
                    const rows = bindContext.$data.rows || bindContext.$parent.dropInRows;
                    const row = rows().find(r => r.id === rowId);
                    setSelectedRow(row.row);

                    // display the selection
                    jElem.addClass('active-details');
                    selectedRow.addClass(selectedClass);
                    detailsElement.insertAfter(selectedRow);
                    detailsElement.fadeIn();
                }

                return false;
            });
        }
    };

    ko.bindingHandlers.detailsHide = {
        init: (element, valueAccessor, allBindings) => {
            $(element).on('click', () => {
                const detailsElement = $(ko.unwrap(valueAccessor)());
                const selectedClass = allBindings.get('selectedClass');
                const clearSelectedRow = allBindings.get('clearRow');

                // clear existing visible selection
                detailsElement.hide();
                $(`.${selectedClass}`).removeClass(selectedClass);
                clearSelectedRow();
                $('.active-details').removeClass('active-details');

                return false;
            });
        }
    };
};
