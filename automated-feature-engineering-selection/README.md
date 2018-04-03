# Automated feature engineering and feature selection

It distributed in reproducible Jupyter notebooks. Quick start:

```bash
git clone -d 1 git@github.com:alexandrnikitin/workshops.git
cd workshops/automated-feature-engineering-selection/
# check the mounted host directory permissions, the docker user should have read/write permissions to the directory; more info https://github.com/jupyter/docker-stacks/tree/master/datascience-notebook
docker run -d -p 8889:8888 -v ~/workshops/automated-feature-engineering-selection/:/home/jovyan/work/automated-feature-engineering-selection -e NB_UID=1000 -e GRANT_SUDO=yes --user root jupyter/datascience-notebook start-notebook.sh --NotebookApp.token=''
sudo pip install -r work/automated-feature-engineering-selection/requirements.txt
# dns issue with docker https://stackoverflow.com/questions/28668180/cant-install-pip-packages-inside-a-docker-container-with-ubuntu
```

Content:

1. Meet `Featuretools` - a python library for automated feature engineering
	1. [Intro](https://github.com/alexandrnikitin/workshops/blob/master/automated-feature-engineering-selection/notebooks/1-featuretools-intro.ipynb)
	2. [Featuretools by example](https://github.com/alexandrnikitin/workshops/blob/master/automated-feature-engineering-selection/notebooks/2-featuretools-by-example.ipynb)
	3. [Scale Featuretools using Dask](https://github.com/alexandrnikitin/workshops/blob/master/automated-feature-engineering-selection/notebooks/3-featuretools-scale.ipynb)
	
2. Automated feature selection

	1. [Examples of statistical, ML algorithms wrappers and embedded techniques.](https://github.com/alexandrnikitin/workshops/blob/master/automated-feature-engineering-selection/notebooks/4-feature-selection.ipynb)
