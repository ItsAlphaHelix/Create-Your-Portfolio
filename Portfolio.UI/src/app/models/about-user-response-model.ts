import { PersonalizeAboutUserRequest } from "./personalize-about-user-request";

export interface AboutUserResponse extends PersonalizeAboutUserRequest {
    email: string,
    jobTitle: string
}