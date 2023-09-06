import { environment } from "src/environments/environment";

//Accounts api end-points.
export const REGISTER_ENDPOINT = `${environment.baseUrlApi}/api/accounts/register`;
export const LOGIN_ENDPOINT = `${environment.baseUrlApi}/api/accounts/login`;
export const GET_USER_ENDPOINT = `${environment.baseUrlApi}/api/accounts/get-user`;

//Homepage api end-points.
export const UPLOAD_PROFILE_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/home/upload-profile-image`;
export const UPLOAD_HOME_PAGE_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/home/upload-homepage-image`;

export const GET_PROFILE_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/home/get-profile-image/`;
export const GET_HOME_PAGE_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/home/get-home-page-image/`;

export const UPDATE_PROFILE_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/home/edit-profile-image`;
export const UPDATE_HOME_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/home/edit-home-image`;

//About users information api end-points
export const ADD_ABOUT_INFORMATION_ENDPOINT = `${environment.baseUrlApi}/api/about-me/add-about`;
export const EDIT_ABOUT_ENDPOINT = `${environment.baseUrlApi}/api/about-me/edit`;
export const UPLOAD_ABOUT_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/about-me/upload-about-image`;

export const GET_ABOUT_ENDPOINT = `${environment.baseUrlApi}/api/about-me/get-about`;
export const GET_ABOUT_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/about-me/get-about-image/`;
export const GET_EDIT_ABOUT_ENDPOINT = `${environment.baseUrlApi}/api/about-me/get-edit-about/`;
export const GET_LANGUAGE_PERCENTAGES_ENDPOINT = `${environment.baseUrlApi}/api/about-me/get-language-percentages`;
export const GET_GITHUB_REPOSITORY_LANGUAGES_ENDPOINT = `${environment.baseUrlApi}/api/about-me/get-github-repo-languages`;

export const UPDATE_ABOUT_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/about-me/edit-about-image`;

//User's project api end-points
export const UPLOAD_MAIN_PROJECT_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/projects/upload-project-image`;
export const GET_ALL_PROJECT_IMAGES_ENDPOINT = `${environment.baseUrlApi}/api/projects/get-all-project-images`;