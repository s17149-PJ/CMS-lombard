export interface User {
  nick: string;
  name?: string;
  surname?: string;
  password?: string;
  email?: string;
  roles?: RoleDefinition[];
  authdata?: string;
  token?: string;
  validUntil?: number;
  success?: boolean;
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
