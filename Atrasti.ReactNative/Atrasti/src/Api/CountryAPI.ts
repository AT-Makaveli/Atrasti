import { HttpServiceSingleton } from "../Services/HttpService";
import { COUNTRY_COUNTRIES } from "./APIData";
import { getErrors } from "../utils/ErrorHelpers";
import { InfoCountry_Res } from "./Models/Responds/InfoCountry_Res";
import { InfoState_Res } from "./Models/Responds/InfoState_Res";
import { InfoCity_Res } from "./Models/Responds/InfoCity_Res";

let countries: InfoCountry_Res[] = [];

export function loadCountries(): Promise<InfoCountry_Res[]> {
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

export function getLoadedCountries(): InfoCountry_Res[] {
    return countries;
}

export function firstCountry(): InfoCountry_Res {
    return countries[0];
}

export function firstState(country: string): InfoState_Res | null {
    const countryRes = findCountry(country);
    if(countryRes === null) return null;

    return countryRes.states[0];
}

export function findCountry(country: string): InfoCountry_Res | null {
    for (const infoCountry of countries) {
        if(infoCountry.country === country) {
            return infoCountry;
        }
    }

    return null;
}

export function findState(countryName: string, stateName: string): InfoState_Res | null {
    const country = findCountry(countryName);
    if(country === null) return null;

    const state = country.states[stateName];
    if(state === undefined) return null;

    return state;
}

export function findCity(countryName: string, stateName: string, cityName: string): InfoCity_Res | null {
    const country = findCountry(countryName);
    if(country === null) return null;

    const state = country.states[stateName];
    if(state === undefined) return null;

    const city = state.cities[cityName];
    if (city === undefined) return null;

    return city;
}
