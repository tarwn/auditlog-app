import dailyEntryChart from './charts/dailyEntryChart';
import inputText from './elements/inputText';
import inputURL from './elements/inputURL';
import sampleViewIFrame from './pages/views/sampleViewIFrame';

// include all components except for pages, which are registered during routing

export default [
    dailyEntryChart,
    inputText,
    inputURL,
    sampleViewIFrame
];
