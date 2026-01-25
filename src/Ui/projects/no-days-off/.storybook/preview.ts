import type { Preview } from '@storybook/angular'
import { setCompodocJson } from "@storybook/addon-docs/angular";
import docJson from "../documentation.json";
setCompodocJson(docJson);

// Import global styles including Material theme
import '../src/styles.scss';

const preview: Preview = {
  parameters: {
    controls: {
      matchers: {
       color: /(background|color)$/i,
       date: /Date$/i,
      },
    },
  },
  decorators: [
    (story) => {
      // Ensure Material Icons stylesheet is loaded
      const existingLink = document.querySelector('link[href*="Material+Icons"]');
      if (!existingLink) {
        const link = document.createElement('link');
        link.href = 'https://fonts.googleapis.com/icon?family=Material+Icons';
        link.rel = 'stylesheet';
        document.head.appendChild(link);
      }

      // Ensure Roboto font is loaded
      const existingRoboto = document.querySelector('link[href*="Roboto"]');
      if (!existingRoboto) {
        const robotoLink = document.createElement('link');
        robotoLink.href = 'https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500&display=swap';
        robotoLink.rel = 'stylesheet';
        document.head.appendChild(robotoLink);
      }

      return story();
    },
  ],
};

export default preview;