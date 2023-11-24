import { ProjectRequest } from "./project-request-model";

export interface ProjectResponse  extends ProjectRequest{
    id: string,
    projectDetailsImageUrl: string
}