import { TableContainer, TableRow, TableCell, TableBody, Table, Paper, TableHead } from "@mui/material";
import Vacunado from "./Vacunado";

import ReactHTMLTableToExcel from "react-html-table-to-excel";
import { CSVLink, CSVDownload } from "react-csv";

const VacunadosTable = ({ vacunados }) => {
  return (
    <>
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
                Fecha vacunacion
              </TableCell>
              <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
                Jurisdiccion
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
    </>
  );
};

export default VacunadosTable;
