import ServiceDirectory from './services/serviceDirectory';
import SiteWideViewModel from './viewmodels/sitewide';
import AppViewModel from './viewmodels/app';

import PageDefinition from './viewmodels/pages/pageDefinition';
import HomePage from './components/pages/homePage';
import APIKeysListPage from './components/pages/apikeys/listAPIKeysPage';
import APIKeysCreatePage from './components/pages/apikeys/createAPIKeysPage';

export default {
    create: () => {
        const services = new ServiceDirectory();
        const context = new SiteWideViewModel(services);
        const viewmodel = new AppViewModel(services, context);

        viewmodel.addPage(new PageDefinition('Home', 'home', '/', HomePage));
        viewmodel.addPage(new PageDefinition('API Keys', 'apikeys', '/configure/apikeys', APIKeysListPage));
        viewmodel.addPage(new PageDefinition('API Keys', 'apikeys', '/configure/apikeys/create', APIKeysCreatePage));
        viewmodel.mount();

        return viewmodel;
    }
};
