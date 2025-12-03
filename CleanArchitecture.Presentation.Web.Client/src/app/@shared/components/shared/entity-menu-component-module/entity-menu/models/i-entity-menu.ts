import { IEntityMenuItem } from './i-entity-menu-item';

export interface IEntityMenu {

  menuTitle?: string;
  menuSubTitle?: string;

  menuItems?: IEntityMenuItem[];
}
