import ServiceDirectory from './services/serviceDirectory';
import SiteWideViewModel from './viewmodels/sitewide';
import AppViewModel from './viewmodels/app';

import PageDefinition from './viewmodels/pages/pageDefinition';
import HomePage from './components/pages/homePage';

export default {
    create: () => {
        const services = new ServiceDirectory();
        const context = new SiteWideViewModel(services);
        const viewmodel = new AppViewModel(services, context);

        viewmodel.addPage(new PageDefinition('Home', 'home', '/', HomePage));
        viewmodel.mount();

        return viewmodel;
    }
};
