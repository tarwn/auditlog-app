import validation from '../extenders/validation';

export default class ViewHeaderLinkModel {
    constructor(rawData, areInputsRequired) {
        this.label = ko.observable(rawData.label).extend(validation.string(areInputsRequired, 40));
        this.url = ko.observable(rawData.url).extend(validation.url(areInputsRequired, 400));

        this.isValid = ko.pureComputed(() => {
            return this.label.isValid() && this.url.isValid();
        });
    }

    static getEmpty(areInputsRequired) {
        return new ViewHeaderLinkModel({}, areInputsRequired);
    }
}
