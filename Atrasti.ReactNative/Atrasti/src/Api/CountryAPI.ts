import { HttpServiceSingleton } from "../Services/HttpService";
import { COUNTRY_COUNTRIES } from "./APIData";
import { getErrors } from "../utils/ErrorHelpers";
import { InfoCountry_Res } from "./Models/Responds/InfoCountry_Res";
import { InfoState_Res } from "./Models/Responds/InfoState_Res";

let countries: InfoCountry_Res[] = [];

export function getCountries(): Promise<InfoCountry_Res[]> {
    return new Promise<InfoCountry_Res[]>((resolve, reject) => {
        if(countries.length !== 0) {
            resolve(countries);
        } else {
            HttpServiceSingleton.get<InfoCountry_Res[]>(COUNTRY_COUNTRIES).then(response => {
                countries = response.data;
                resolve(countries);
            }).catch(error => {
                reject(getErrors(error.response.data));
            });
        }
    });
}

export function findCountry(country: string): InfoCountry_Res | null {
    for (const infoCountry of countries) {
        if(infoCountry.country === country) {
            return infoCountry;
        }
    }

    return null;
}

export function findState(county: string): InfoState_Res | null {
    for (const infoCountry of countries) {

        for (const infoState of Object.values(infoCountry.states)) {
            if (infoState.state === county) {
                return infoState;
            }
        }
    }

    return null;
}
