import type { Meta, StoryObj } from '@storybook/angular';
import { SecondaryHeader } from './secondary-header';
import { provideRouter } from '@angular/router';
import { applicationConfig } from '@storybook/angular';

const meta: Meta<SecondaryHeader> = {
  title: 'Components/SecondaryHeader',
  component: SecondaryHeader,
  decorators: [
    applicationConfig({
      providers: [provideRouter([])]
    })
  ],
  tags: ['autodocs'],
  argTypes: {
    title: {
      control: 'text',
      description: 'Header title'
    },
    backLink: {
      control: 'text',
      description: 'Back link URL'
    }
  }
};

export default meta;
type Story = StoryObj<SecondaryHeader>;

export const Default: Story = {
  args: {
    title: 'Page Title',
    backLink: ''
  }
};

export const WithBackLink: Story = {
  args: {
    title: 'Page Title',
    backLink: '/'
  }
};
