export const urlBase = "https://localhost:44385/api";

const allUrls = {
  user: `${urlBase}/Usuarios/`,
  jurisdiccion: `${urlBase}/Jurisdicciones/`,
  roles: `${urlBase}/Roles/`,
  vacunas: `${urlBase}/TiposVacunas/`,
  pandemias: `${urlBase}/Pandemias/`,
  calendario: `${urlBase}/Vacunas/GetDescripcionesVacunasCalendario`,
  crearVacuna: `${urlBase}/Vacunas/CrearVacuna`,
  descripcionAnual: `${urlBase}/Vacunas/GetDescripcionesVacunasAnuales`,
  todasVacunas: `${urlBase}/Vacunas/GetAll`,
  marcasComerciales: `${urlBase}/MarcasComerciales/GetAll`,
  marcasComercialesModificar: `${urlBase}/MarcasComerciales/ModificarMarcaComercial`,
  marcasComercialesCrear: `${urlBase}/MarcasComerciales/CrearMarcaComercial`,
  crearVacunaDesarrollada: `${urlBase}/VacunasDesarrolladas/CrearVacunaDesarrollada`,
  todasVacunasDesarrolladas: `${urlBase}/VacunasDesarrolladas/getAll`,
  todasVacunasDesarrolladasHabilitadas: `${urlBase}/VacunasDesarrolladas/GetAllActivas`,
  todasVacunasEliminadas: `${urlBase}/VacunasDesarrolladas/GetAllEliminados`,
  eliminarVacunaDesarrollada: `${urlBase}/VacunasDesarrolladas/DeleteVacunaDesarrollada`,
  apiRenaper: `https://api.claudioraverta.com/personas/`,
  consultarVacunacion: `${urlBase}/VacunasAplicadas/ConsultarVacunacion`,
  crearVacunacion: `${urlBase}/VacunasAplicadas/CrearVacunacion`,
  vacunasAplidas: `${urlBase}/VacunasAplicadas/GetAll`,
};
export default allUrls;
