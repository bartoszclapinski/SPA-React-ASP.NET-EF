import {House} from "../types/house";
import {useEffect, useState} from "react";
import {config} from "../config";

const useFetchHouses = (): House[] => {
    const [houses, setHouses] = useState<House[]>([]);

    useEffect(() => {
        const fetchHouses = async () => {
            const response = await fetch(`${config.baseApiUrl}/houses`);
            const houses = await response.json();
            setHouses(houses);
        };

        fetchHouses().then();

    }, [])    ;

    return houses;
}

export default useFetchHouses;