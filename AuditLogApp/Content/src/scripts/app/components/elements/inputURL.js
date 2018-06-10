export default {
    name: 'input-url',
    viewModel: class InputURL {
        constructor(params) {
            if (typeof params.value !== 'function') {
                throw new Error(`Value is not an observable, InputURL: ${ko.toJSON(params)}`);
            }

            this.id = params.id;
            this.value = params.value;
            this.maxLength = params.maxLength;
            // don't call this class, css: class as a binding fails for some reason
            this.extraClass = ko.pureComputed(() => params.class);
            this.isValid = ko.pureComputed(() => {
                // https://github.com/jquery-validation/jquery-validation/blob/c1db10a34c0847c28a5bd30e3ee1117e137ca834/src/core.js#L1349
                const regex = /^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})).?)(?::\d{2,5})?(?:[/?#]\S*)?$/;

                return this.value() != null &&
                    this.value().length <= this.maxLength &&
                    regex.test(this.value());
            });
        }
    },
    template: `
        <input type="text" class="ala-form-input" data-bind="attr: { id: id }, value: value, css: { 'ala-input-error': !isValid() }, css: extraClass" />
    `
};
