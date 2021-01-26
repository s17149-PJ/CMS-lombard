import { User } from './../model/auth.model';
export interface LombardProduct {
  id: number;
  name: string;
  category: LompardProductCategory;
  publishDate: number;
  finallizationDateTime: number;
  finallizationDateTimeDouble: number;
  description: string;
  image?: string;
  imageMetaData?: string;
  numberOfRatings?: number;
  ratingAvarage?: number;
  startingBid?: ItemBid;
  winningBid?: ItemBid;
  bids?: ItemBid[];
}

export interface ItemBid {
  id: number;
  item?: LombardProduct;
  money: number;
  createdOn?: number;
  isRating: boolean;
  user?: User;
}

export interface LompardProductCategory {
  name: string;
}

export interface Bid {
  userId: number;
  token: string;
  subjectId: number;
  money: number;
  isRating: boolean;
}

export interface Bid {
  userId: number;
  token: string;
  subjectId: number;
  money: number;
  isRating: boolean;
}


export interface Tag {
  id: number;
  name: string
}
