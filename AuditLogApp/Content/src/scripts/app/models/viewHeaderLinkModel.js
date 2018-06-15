import validation from '../extenders/validation';

export default class ViewHeaderLinkModel {
    constructor(rawData, areInputsRequired) {
        const data = rawData || {};

        this.label = ko.observable(data.label).extend(validation.string(areInputsRequired, 40));
        this.url = ko.observable(data.url).extend(validation.url(areInputsRequired, 400));

        this.isValid = ko.pureComputed(() => {
            return this.label.isValid() && this.url.isValid();
        });
    }
}
