import {House} from "../types/house";
//import {useEffect, useState} from "react";
import {config} from "../config";
import {useQuery, useQueryClient, useMutation} from "react-query";
import axios, {AxiosError, AxiosResponse} from "axios";
import {useNavigate} from "react-router-dom";

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

const useAddHouse = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError, House>(
        (h) => axios.post(`${config.baseApiUrl}/houses`, h),
        {
            onSuccess: () => {
                queryClient.invalidateQueries("houses").then();
                nav("/");
            }
        }
    );
};

const useUpdateHouse = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError, House>(
        (h) => axios.put(`${config.baseApiUrl}/houses`, h),
        {
            onSuccess: (_, house) => {
                queryClient.invalidateQueries("houses").then();
                nav(`/house/${house.houseId}`);
            }
        }
    );
};

const useDeleteHouse = () => {
    const nav = useNavigate();
    const queryClient = useQueryClient();
    return useMutation<AxiosResponse, AxiosError, House>(
        (h) => axios.delete(`${config.baseApiUrl}/houses/${h.houseId}`),
        {
            onSuccess: () => {
                queryClient.invalidateQueries("houses").then();
                nav("/");
            }
        }
    );
};

export default useFetchHouses;
export { useFetchHouse, useAddHouse, useUpdateHouse, useDeleteHouse };