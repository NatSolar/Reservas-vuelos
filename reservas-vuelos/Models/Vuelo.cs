using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace reservas_vuelos.Models
{
    public class Vuelo
    {
        public int flightNo { get; set; }
        public Boolean oneWay { get; set; }
        public Decimal duration { get; set; }
        public string departureIataCode { get; set; }
        public string departureTerminal { get; set; }
        public DateTime departureDate { get; set; }

        public string arrivalIataCode { get; set; }
        public DateTime arrivalDate { get; set; }
        public string carrierCode { get; set; }
        public string carrierName { get; set; }
        public string aircraftType { get; set; }
        public int stopsNo { get; set; }

        public string currency { get; set; }
        public Decimal total { get; set; }
        public string cabin { get; set; }
        public string cabinFare { get; set; }


        public Vuelo(int flight_no, Boolean one_way, Decimal durations, string departure_iata_code, string departure_terminal,
            DateTime departure_date, string arrival_iata_code, DateTime arrival_date, string carrier_code, string carrier_name,
            string aircraft_type, int stops_no, string currencys, Decimal totals, string cabins, string cabin_fare)
        {
            this.flightNo = flight_no;
            this.oneWay = one_way;
            this.duration = durations;
            this.departureIataCode = departure_iata_code;
            this.departureTerminal = departure_terminal;
            this.departureDate = departure_date;
            this.arrivalIataCode = arrival_iata_code;
            this.arrivalDate = arrival_date;
            this.carrierCode = carrier_code;
            this.carrierName = carrier_name;
            this.aircraftType = aircraft_type;
            this.stopsNo = stops_no;
            this.currency = currencys;
            this.total = totals;
            this.cabin = cabins;
            this.cabinFare = cabin_fare;
        }

        public Vuelo()
        {

        }
    }


}