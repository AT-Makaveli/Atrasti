import { UserToken } from "./Models/UserToken";
import AsyncStorage from "@react-native-async-storage/async-storage";

export class UserService {
    private _user: UserToken | null = null;
    private _refreshToken: string | null = null;

    public isSignedIn(): boolean {
        return this._user != null;
    }

    public getToken(): string {
        return this._user?.token as string
    }

    public async signIn(data: UserToken) {
        this._user = data;
        this._refreshToken = data.refreshToken;
        await AsyncStorage.setItem('refresh_token', this._refreshToken);
    }
}

export const UserServiceSingleton = new UserService();
