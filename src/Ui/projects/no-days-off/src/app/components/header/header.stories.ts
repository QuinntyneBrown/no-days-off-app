import type { Meta, StoryObj } from '@storybook/angular';
import { Header } from './header';
import { provideRouter } from '@angular/router';
import { applicationConfig } from '@storybook/angular';

const meta: Meta<Header> = {
  title: 'Components/Header',
  component: Header,
  decorators: [
    applicationConfig({
      providers: [provideRouter([])]
    })
  ],
  tags: ['autodocs'],
  argTypes: {
    isAuthenticated: {
      control: 'boolean',
      description: 'Whether the user is authenticated'
    }
  }
};

export default meta;
type Story = StoryObj<Header>;

export const Authenticated: Story = {
  args: {
    isAuthenticated: true
  }
};

export const NotAuthenticated: Story = {
  args: {
    isAuthenticated: false
  }
};
