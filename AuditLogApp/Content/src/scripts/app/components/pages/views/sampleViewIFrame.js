export default {
    name: 'sample-view-iframe',
    viewModel: class SampleIFrame {
        constructor(params) {
            if (params.subscribable === undefined) {
                throw new Error('Subscribable not provided to Sample View IFrame Component');
            }
            if (ko[params.subscribable] === undefined) {
                throw new Error('Subscribable not available for Sample View IFrame Component');
            }
            this.id = params.id;
            this.latestMessage = params.initialData;

            ko[params.subscribable].subscribe((newValue) => {
                this.latestMessage = newValue;
                this.sendMessage(newValue);
            });

            window.addEventListener('message', this.receiveMessage.bind(this), false);
        }

        sendMessage(message) {
            const frame = window.frames[this.id];
            if (frame) {
                const messageJson = ko.toJSON(message);
                frame.contentWindow.postMessage(messageJson, window.location.origin);
            }
        }

        receiveMessage(message) {
            if (message.origin !== window.location.origin) {
                return;
            }

            if (message.data === 'ready') {
                this.sendMessage(this.latestMessage);
            }
        }
    },
    template: `
        <iframe src="/dropin/index.html" class="ala-view-edit-preview-iframe" data-bind="attr: { id: id }" />
    `
};
