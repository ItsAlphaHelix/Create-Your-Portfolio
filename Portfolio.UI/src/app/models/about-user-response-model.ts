import { PersonalizeAboutUserRequest } from "./personalize-about-user-request";

export interface AboutUserResponse extends PersonalizeAboutUserRequest {
    id: string,
    email: string,
    jobTitle: string
}