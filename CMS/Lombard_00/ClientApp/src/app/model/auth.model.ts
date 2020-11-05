export interface User {
  username: string;
  password?: string;
  firstName: string;
  lastName: string;
  email: string;
  roles: string[];
  authdata?: string;
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
