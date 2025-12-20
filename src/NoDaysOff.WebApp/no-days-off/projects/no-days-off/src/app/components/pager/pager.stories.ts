import type { Meta, StoryObj } from '@storybook/angular';
import { Pager } from './pager';
import { action } from '@storybook/addon-actions';

const meta: Meta<Pager> = {
  title: 'Components/Pager',
  component: Pager,
  tags: ['autodocs'],
  argTypes: {
    pageNumber: {
      control: { type: 'number', min: 1 },
      description: 'Current page number'
    },
    totalPages: {
      control: { type: 'number', min: 1 },
      description: 'Total number of pages'
    }
  }
};

export default meta;
type Story = StoryObj<Pager>;

export const Default: Story = {
  args: {
    pageNumber: 1,
    totalPages: 10
  }
};

export const MiddlePage: Story = {
  args: {
    pageNumber: 5,
    totalPages: 10
  }
};

export const LastPage: Story = {
  args: {
    pageNumber: 10,
    totalPages: 10
  }
};

export const SinglePage: Story = {
  args: {
    pageNumber: 1,
    totalPages: 1
  }
};
