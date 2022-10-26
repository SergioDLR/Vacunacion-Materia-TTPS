import axios from "axios";
import { useState, useEffect, useContext } from "react";
import { UserContext } from "../Context/UserContext";
import allUrls from "@/services/backend_url";

const Vacunados = () => {
  const { userSesion } = useContext(UserContext);
  useEffect(() => {
    axios.get(`${allUrls.vacunasAplidas}?emailUsuario=${userSesion.email}`).then((response) => console.log(response));
  }, []);
  return <>Vacunados:</>;
};

export default Vacunados;
