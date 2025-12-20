import type { Meta, StoryObj } from '@storybook/angular';
import { HamburgerButton } from './hamburger-button';

const meta: Meta<HamburgerButton> = {
  title: 'Components/HamburgerButton',
  component: HamburgerButton,
  tags: ['autodocs'],
  argTypes: {
    isOpen: {
      control: 'boolean',
      description: 'Whether the menu is open'
    }
  }
};

export default meta;
type Story = StoryObj<HamburgerButton>;

export const Closed: Story = {
  args: {
    isOpen: false
  }
};

export const Open: Story = {
  args: {
    isOpen: true
  }
};
