import PageBase from './pageBase';

export default {
    name: 'page-home',
    viewModel: class HomePage extends PageBase {
        constructor(params) {
            super(params);
            this.initialize();
        }

        initialize() {
            setTimeout(() => {
                this.readyForDisplay(true);
            }, 3000);
        }

        // dispose() {
        // }
    },
    template: `
        <div class="ala-loading" data-bind="visible: !readyForDisplay()">Loading...</div>
        <div data-bind="if: readyForDisplay">
            stuff
        </div>
    `
};
