export default class Validated {
    static applyExtender(ko) {
        /* eslint-disable no-param-reassign, max-len */
        ko.extenders.validated = (target, args) => {
            const isValid = ko.observable(false);

            const result = ko.pureComputed({
                read: target,
                write: (rawValue) => {
                    const currentValue = target();
                    const newValue = rawValue !== undefined ? rawValue : args.default;

                    isValid(args.validationFunction(newValue, currentValue, args.validationConfig));

                    if (isValid) {
                        target(newValue);
                    }
                }
            }).extend({ notify: 'always' });
            result.isValid = isValid;

            result(target());

            return result;
        };
        /* eslint-enable no-param-reassign, max-len */
    }

    static string(isRequired, maxLength) {
        return {
            validated: {
                validationFunction: Validated.stringIsValid,
                validationConfig: { isRequired, maxLength },
                default: ''
            }
        };
    }

    static stringIsValid(newValue, currentValue, config) {
        return newValue != null &&
            (!config.isRequired || newValue.length > 0) &&
            newValue.length <= config.maxLength;
    }

    static url(isRequired, maxLength) {
        return {
            validated: {
                validationFunction: Validated.urlIsValid,
                validationConfig: { isRequired, maxLength },
                default: ''
            }
        };
    }

    static urlIsValid(newValue, currentValue, config) {
        // https://github.com/jquery-validation/jquery-validation/blob/c1db10a34c0847c28a5bd30e3ee1117e137ca834/src/core.js#L1349
        const regex = /^(http:\/\/|https:\/\/|mailto:[^@]+@)[a-z0-9\u00a1-\uffff-]+(\.[a-z0-9\u00a1-\uffff-]+)*(\.[a-z]{2,5})?(:[0-9]{1,5})?(\/.*)?$/;

        return Validated.stringIsValid(newValue, currentValue, config) &&
            ((!config.isRequired && newValue.length === 0) || regex.test(newValue));
    }
}
