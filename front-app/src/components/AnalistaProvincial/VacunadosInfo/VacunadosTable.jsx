import { TableContainer, TableRow, TableCell, TableBody, Table, Paper, TableHead } from "@mui/material";
import Vacunado from "./Vacunado";
import axios from "axios";
import ReactHTMLTableToExcel from "react-html-table-to-excel";
import { CSVLink } from "react-csv";
import { useContext } from "react";
import { UserContext } from "@/components/Context/UserContext";
import CustomButton from "@/components/utils/CustomButtom";
import { useAlert } from "react-alert";
import allUrls from "@/services/backend_url";

const VacunadosTable = ({ vacunados }) => {
  const alert = useAlert();
  const { userSesion } = useContext(UserContext);
  const handleETL = () => {
    try {
      axios
        .get(`${allUrls.etl}?emailOperadorNacional=${userSesion.email}`)
        .then((response) => alert.success("Se ejecuto el etl correctamente"))
        .catch((err) => alert.error("Ocurrio un error"));
    } catch {
      alert.error("Ocurrio un error en el servidor");
    }
  };
  return (
    <div style={{ minHeight: 600 }}>
      <ReactHTMLTableToExcel
        id="test-table-xls-button"
        className="btn"
        table="table-to-xls"
        filename="vacunados"
        sheet="vacunados"
        buttonText="Descargar en formato excel"
      />

      <CSVLink data={vacunados} filename={"vacunados.csv"}>
        <button className="btn">Descargar en formato CSV</button>
      </CSVLink>
      <CustomButton onClick={handleETL} sx={{ marginLeft: 10 }} variant={"contained"} color={"success"} type={"submit"}>
        Ejecutar ETL
      </CustomButton>
      <TableContainer component={Paper} sx={{ marginTop: 1 }}>
        <Table id={"table-to-xls"} sx={{ minWidth: 650 }} size="small">
          <TableHead sx={{ backgroundColor: "#2E7994" }}>
            <TableRow>
              <TableCell sx={{ color: "white", fontWeight: 600 }}>DNI</TableCell>
              <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
                Nombre
              </TableCell>
              <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
                Apellido
              </TableCell>
              <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
                Fecha vacunación
              </TableCell>
              <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
                Jurisdicción
              </TableCell>
              <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
                Vacuna
              </TableCell>
              <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
                Marca comercial
              </TableCell>
              <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
                Dosis
              </TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {vacunados.map((element, index) => (
              <Vacunado vacunado={element} key={index} />
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </div>
  );
};

export default VacunadosTable;
