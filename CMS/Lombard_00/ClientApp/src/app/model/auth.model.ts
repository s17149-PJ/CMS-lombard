export interface User {
  nick: string;
  password?: string;
  firstName?: string;
  lastName?: string;
  email?: string;
  roles?: string[];
  authdata?: string;
  success: boolean;
}

export interface RoleDefinition {
  id: number;
  name: string;
}

export const RoleDefinitions: RoleDefinition[] = [
  {
    id: 1,
    name: 'ADMIN',
  },
  {
    id: 2,
    name: 'USER',
  },
];
