export const config = {
    baseApiUrl: "https://localhost:4000"
}
export const currencyFormatter = Intl.NumberFormat("en-US", {
   style: "currency",
   currency: "USD",
   maximumFractionDigits: 2
});