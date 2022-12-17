import { useState, useEffect, useContext } from "react";
import { UserContext } from "@/components/Context/UserContext";
import { Container } from "@mui/system";
import axios from "axios";
import allUrls from "@/services/backend_url";
import {
  Table,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  TableCell,
  TableBody,
  SvgIcon,
  Divider,
} from "@mui/material";
import CustomLoader from "@/components/utils/CustomLoader";
import CustomButton from "@/components/utils/CustomButtom";
import VisibilityIcon from "@mui/icons-material/Visibility";
import numberParser from "@/components/utils/numberParser";
import CustomModal from "@/components/utils/Modal";
const StockContainer = ({ title, url, param = "emailOperadorNacional", url2 }) => {
  const { userSesion } = useContext(UserContext);
  const [stock, setStock] = useState([]);
  const [stockParaVacunacion, setStockParaVacunacion] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);
  const [jurisdiccionSeleccionada, setJurisdiccionSeleccionada] = useState([]);
  const [open, setOpen] = useState(false);

  useEffect(() => {
    axios
      .get(`${url}?${param}=${userSesion.email}`)
      .then((response) => {
        setStock(response.data);
      })
      .catch(() => alert("Ocurrio un error"))
      .finally(() => setEstaCargando(false));
    if (url2) {
      axios
        .get(`${url2}?${param}=${userSesion.email}`)
        .then((response) => {
          setStockParaVacunacion(response.data.vacunasStockOperadorNacionalDTO);
        })
        .catch(() => alert("Ocurrio un error"))
        .finally(() => setEstaCargando(false));
    }
  }, []);

  const hanldeSelection = (element) => {
    //TODO: Disclaimer: Este probablemenete sea el algoritmo mas horrendo que vas a ver en tu vida
    const newArr = [];
    element.listaVacunasStockDTOJurisdiccion.forEach((element) => {
      let newEle = {
        nombre: element.descripcion.split("-", 1).shift(),
        cant: parseInt(element.descripcion.split(" ").pop()),
        idVacuna: element.idVacuna,
      };
      const search = newArr.find((e) => e.idVacuna === element.idVacuna);
      const searchIndex = newArr.findIndex((e) => e.idVacuna === element.idVacuna);
      if (search) {
        newArr[searchIndex].cant = newEle.cant + newArr[searchIndex].cant;
      } else {
        newArr.push(newEle);
      }
    });
    setJurisdiccionSeleccionada(newArr);
    setOpen(true);
  };
  return (
    <Container>
      {estaCargando && <CustomLoader />}
      <h3>{title}</h3>
      <TableContainer component={Paper} sx={{ marginTop: 1 }}>
        <Table sx={{ minWidth: 650 }} size="small">
          <TableHead sx={{ backgroundColor: "#2E7994" }}>
            <TableRow>
              <TableCell sx={{ color: "white", fontWeight: 600 }}>Descripción</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {stock.map((element, index) => (
              <TableRow key={index}>
                <TableCell>{element.descripcion}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      {url2 && (
        <>
          <h3>Stock para vacunación:</h3>
          <TableContainer component={Paper} sx={{ marginTop: 1 }}>
            <Table sx={{ minWidth: 650 }} size="small">
              <TableHead sx={{ backgroundColor: "#2E7994" }}>
                <TableRow>
                  <TableCell sx={{ color: "white", fontWeight: 600 }}>Jurisdicción</TableCell>
                  <TableCell sx={{ color: "white", fontWeight: 600 }}>Total</TableCell>
                  <TableCell sx={{ color: "white", fontWeight: 600 }}>Detalles</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {stockParaVacunacion.map((element, index) => (
                  <>
                    <TableRow key={index}>
                      <TableCell>{element.descripcionJurisdiccion}</TableCell>
                      <TableCell>{numberParser(element.totalJurisdiccionDisponible)}</TableCell>
                      <TableCell>
                        {element.totalJurisdiccionDisponible > 0 && (
                          <CustomButton
                            variant={"outlined"}
                            color={"info"}
                            textColor={"#3ee8e5"}
                            onClick={() => hanldeSelection(element)}
                          >
                            <SvgIcon>
                              <VisibilityIcon></VisibilityIcon>
                            </SvgIcon>
                          </CustomButton>
                        )}
                      </TableCell>
                    </TableRow>
                  </>
                ))}
              </TableBody>
            </Table>
            <CustomModal open={open} setOpen={setOpen} displayButton={false}>
              {jurisdiccionSeleccionada.map((element, index) => (
                <div key={index}>
                  <p>
                    {element.nombre} - cantidad: {element.cant}
                  </p>
                  <Divider></Divider>
                </div>
              ))}
              <CustomButton variant={"outlined"} color={"error"} textColor="red" onClick={() => setOpen(false)}>
                Cerrar
              </CustomButton>
            </CustomModal>
          </TableContainer>
        </>
      )}
    </Container>
  );
};

export default StockContainer;
