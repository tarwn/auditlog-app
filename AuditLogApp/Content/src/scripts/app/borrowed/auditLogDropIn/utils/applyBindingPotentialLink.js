/* eslint-disable no-param-reassign */

export default (ko, $) => {
    ko.bindingHandlers.potentialLink = {
        // init: (element, valueAccessor, allBindings, viewModel, bindContext) => { },
        // update: (element, valueAccessor, allBindings, viewModel, bindContext) => { }
        update: (element, valueAccessor) => {
            const properties = ko.unwrap(valueAccessor)();

            if (properties.url == null || properties.url === '') {
                $(element).text(properties.text);
            }
            else {
                $(element).html(`<a href="${properties.url}" title="View details">${properties.text} <i class="icon-link-ext"></i></a>`);
            }
        }
    };
};
