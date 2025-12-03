import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProfilePictureService {

    constructor() { }


    public getPicture(): string | null {
        return localStorage.getItem("profilePicture");
    }

    public resetProfilePicture(): void {
        localStorage.removeItem("profilePicture");
    }

    public savePicture(profilePicture: string): void {
        localStorage.setItem("profilePicture", profilePicture);
    }
}
