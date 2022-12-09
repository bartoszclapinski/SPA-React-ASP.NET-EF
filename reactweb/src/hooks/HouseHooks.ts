import {House} from "../types/house";
//import {useEffect, useState} from "react";
import {config} from "../config";
import {useQuery} from "react-query";
import axios, {AxiosError} from "axios";

const useFetchHouses = () => {
    return useQuery<House[], AxiosError>("houses", () =>
        axios.get(`${config.baseApiUrl}/houses`).then((response) => response.data));

    /*
    const [houses, setHouses] = useState<House[]>([]);

    useEffect(() => {
        const fetchHouses = async () => {
            const response = await fetch(`${config.baseApiUrl}/houses`);
            const houses = await response.json();
            setHouses(houses);
        };

        fetchHouses().then();

    }, []);

    return houses;
     */
}

const useFetchHouse = (id: number) => {
    return useQuery<House, AxiosError>(["houses", id], () =>
        axios.get(`${config.baseApiUrl}/house/${id}`).then((response) => response.data)
    );
}

export default useFetchHouses;
export { useFetchHouse };