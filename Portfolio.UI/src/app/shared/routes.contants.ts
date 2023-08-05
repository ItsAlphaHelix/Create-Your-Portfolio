import { environment } from "src/environments/environment";

//Account api end-points.
export const REGISTER_ENDPOINT = `${environment.baseUrlApi}/api/accounts/register`
export const LOGIN_ENDPOINT = `${environment.baseUrlApi}/api/accounts/login`
export const UPLOAD_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/user-profile/upload-image`
export const GET_USER_PROFILE_IMAGE_ENDPOINT = `${environment.baseUrlApi}/api/get-profile-image/`