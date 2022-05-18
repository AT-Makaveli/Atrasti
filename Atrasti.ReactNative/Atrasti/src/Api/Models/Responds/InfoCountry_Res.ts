
import { InfoState_Res } from './InfoState_Res';

export interface InfoCountry_Res {
    country: string;
    states: { [key: string]: InfoState_Res };
}