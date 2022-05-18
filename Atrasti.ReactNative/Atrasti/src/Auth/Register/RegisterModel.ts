export default class RegisterModel {
    private static _email: string;
    private static _firstName: string;
    private static _lastName: string;
    private static _company: string;
    private static _password: string;

    static get email(): string {
        return this._email;
    }

    static set email(value: string) {
        this._email = value;
    }

    static get firstName(): string {
        return this._firstName;
    }

    static set firstName(value: string) {
        this._firstName = value;
    }

    static get lastName(): string {
        return this._lastName;
    }

    static set lastName(value: string) {
        this._lastName = value;
    }

    static get company(): string {
        return this._company;
    }

    static set company(value: string) {
        this._company = value;
    }

    static get password(): string {
        return this._password;
    }

    static set password(value: string) {
        this._password = value;
    }
}
