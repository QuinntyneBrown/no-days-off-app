import type { Meta, StoryObj } from '@storybook/angular';
import { ModalWindow } from './modal-window';

const meta: Meta<ModalWindow> = {
  title: 'Components/ModalWindow',
  component: ModalWindow,
  tags: ['autodocs'],
  argTypes: {
    isOpen: {
      control: 'boolean',
      description: 'Whether the modal is open'
    },
    title: {
      control: 'text',
      description: 'Modal title'
    }
  }
};

export default meta;
type Story = StoryObj<ModalWindow>;

export const Closed: Story = {
  args: {
    isOpen: false,
    title: 'Modal Title'
  }
};

export const Open: Story = {
  args: {
    isOpen: true,
    title: 'Modal Title'
  }
};

export const OpenWithLongTitle: Story = {
  args: {
    isOpen: true,
    title: 'This is a very long modal title that should wrap properly'
  }
};

export const OpenWithNoTitle: Story = {
  args: {
    isOpen: true,
    title: ''
  }
};
