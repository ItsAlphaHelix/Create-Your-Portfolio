import { AboutInformationRequest } from "./about-request-model";

export interface AboutInformationResponse extends AboutInformationRequest {
    id: string,
    email: string,
    jobTitle: string
}