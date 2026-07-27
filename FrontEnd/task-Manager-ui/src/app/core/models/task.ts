import { TaskStatus } from '../enums/task-status';

export interface Task {
  id: number;
  title: string;
  description?: string;
  status: TaskStatus;
  dueDate?: Date;
  projectId: number;
}