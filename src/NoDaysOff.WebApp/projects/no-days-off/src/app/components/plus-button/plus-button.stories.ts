import type { Meta, StoryObj } from '@storybook/angular';
import { PlusButton } from './plus-button';

const meta: Meta<PlusButton> = {
  title: 'Components/PlusButton',
  component: PlusButton,
  tags: ['autodocs'],
  parameters: {
    layout: 'centered'
  }
};

export default meta;
type Story = StoryObj<PlusButton>;

export const Default: Story = {};
