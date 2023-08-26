import { environment } from "src/environments/environment";

//Accounts api end-points.
export const REGISTER_ENDPOINT = `${environment.baseUrlApi}/api/accounts/register`;
export const LOGIN_ENDPOINT = `${environment.baseUrlApi}/api/accounts/login`;
export const GET_USER_ENDPOINT = `${environment.baseUrlApi}/api/accounts/get-user`;

//Users profile api end-points.
export const UPLOAD_PROFILE_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/users-profile/upload-profile-image`;
export const UPLOAD_HOME_PAGE_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/users-profile/upload-homepage-image`;
export const GET_USER_PROFILE_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/users-profile/get-profile-image/`;
export const PERSONALIZE_ABOUT_USER_ENDPOINT = `${environment.baseUrlApi}/api/users-profile/personalize-about`;
export const GET_ABOUT_USER_ENDPOINT = `${environment.baseUrlApi}/api/users-profile/get-about`;
export const UPLOAD_ABOUT_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/users-profile/upload-about-image`;
export const GET_ABOUT_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/users-profile/get-about-image/`;