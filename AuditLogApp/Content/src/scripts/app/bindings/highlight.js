export default function applyExtender(ko) {
    /* eslint-disable no-param-reassign, max-len */
    ko.bindingHandlers.highlight = {
        update: (element, valueAccessor) => {
            // no I don't know why the valueAccessor is so deeply nested...
            const val = ko.unwrap(valueAccessor)();
            $(element).text(val());
            hljs.highlightBlock(element);
        }
    };
    /* eslint-enable no-param-reassign, max-len */
}

